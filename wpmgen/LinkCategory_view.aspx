<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkCategory_view.aspx.vb" Inherits="LinkCategory_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkCategory.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_view = new ew_Page("LinkCategory_view");
// page properties
LinkCategory_view.PageID = "view"; // page ID
var EW_PAGE_ID = LinkCategory_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Part Category
<br /><br />
<% If LinkCategory.Export = "" Then %>
<a href="LinkCategory_list.aspx">Back to List</a>&nbsp;
<a href="<%= LinkCategory.AddUrl %>">Add</a>&nbsp;
<a href="<%= LinkCategory.EditUrl %>">Edit</a>&nbsp;
<a href="<%= LinkCategory.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= LinkCategory.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% LinkCategory_view.ShowMessage() %>
<p />
<% If LinkCategory.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkCategory_view.Pager Is Nothing Then LinkCategory_view.Pager = New cPrevNextPager(LinkCategory_view.lStartRec, LinkCategory_view.lDisplayRecs, LinkCategory_view.lTotalRecs) %>
<% If LinkCategory_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkCategory_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkCategory_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkCategory_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkCategory_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkCategory_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkCategory_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkCategory_view.sSrchWhere = "0=101" Then %>
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
<% If LinkCategory.Title.Visible Then ' Title %>
	<tr<%= LinkCategory.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= LinkCategory.Title.CellAttributes %>>
<div<%= LinkCategory.Title.ViewAttributes %>><%= LinkCategory.Title.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkCategory.Description.Visible Then ' Description %>
	<tr<%= LinkCategory.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= LinkCategory.Description.CellAttributes %>>
<div<%= LinkCategory.Description.ViewAttributes %>><%= LinkCategory.Description.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkCategory.ParentID.Visible Then ' ParentID %>
	<tr<%= LinkCategory.ParentID.RowAttributes %>>
		<td class="ewTableHeader">Parent</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
<div<%= LinkCategory.ParentID.ViewAttributes %>><%= LinkCategory.ParentID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkCategory.zPageID.Visible Then ' PageID %>
	<tr<%= LinkCategory.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
<div<%= LinkCategory.zPageID.ViewAttributes %>><%= LinkCategory.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If LinkCategory.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkCategory_view.Pager Is Nothing Then LinkCategory_view.Pager = New cPrevNextPager(LinkCategory_view.lStartRec, LinkCategory_view.lDisplayRecs, LinkCategory_view.lTotalRecs) %>
<% If LinkCategory_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkCategory_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkCategory_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkCategory_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkCategory_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkCategory_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkCategory_view.PageUrl %>start=<%= LinkCategory_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkCategory_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkCategory_view.sSrchWhere = "0=101" Then %>
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
<% If LinkCategory.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
