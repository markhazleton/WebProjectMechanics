<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkType_view.aspx.vb" Inherits="LinkType_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkType_view = new ew_Page("LinkType_view");
// page properties
LinkType_view.PageID = "view"; // page ID
var EW_PAGE_ID = LinkType_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkType_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkType_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Part Type
<br /><br />
<% If LinkType.Export = "" Then %>
<a href="LinkType_list.aspx">Back to List</a>&nbsp;
<a href="<%= LinkType.AddUrl %>">Add</a>&nbsp;
<a href="<%= LinkType.EditUrl %>">Edit</a>&nbsp;
<a href="<%= LinkType.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= LinkType.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% LinkType_view.ShowMessage() %>
<p />
<% If LinkType.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkType_view.Pager Is Nothing Then LinkType_view.Pager = New cPrevNextPager(LinkType_view.lStartRec, LinkType_view.lDisplayRecs, LinkType_view.lTotalRecs) %>
<% If LinkType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkType_view.sSrchWhere = "0=101" Then %>
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
<% If LinkType.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= LinkType.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type CD</td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeDesc.Visible Then ' LinkTypeDesc %>
	<tr<%= LinkType.LinkTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Link Type Desc</td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
<div<%= LinkType.LinkTypeDesc.ViewAttributes %>><%= LinkType.LinkTypeDesc.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeComment.Visible Then ' LinkTypeComment %>
	<tr<%= LinkType.LinkTypeComment.RowAttributes %>>
		<td class="ewTableHeader">Link Type Comment</td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>>
<div<%= LinkType.LinkTypeComment.ViewAttributes %>><%= LinkType.LinkTypeComment.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeTarget.Visible Then ' LinkTypeTarget %>
	<tr<%= LinkType.LinkTypeTarget.RowAttributes %>>
		<td class="ewTableHeader">Link Type Target</td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
<div<%= LinkType.LinkTypeTarget.ViewAttributes %>><%= LinkType.LinkTypeTarget.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If LinkType.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkType_view.Pager Is Nothing Then LinkType_view.Pager = New cPrevNextPager(LinkType_view.lStartRec, LinkType_view.lDisplayRecs, LinkType_view.lTotalRecs) %>
<% If LinkType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkType_view.PageUrl %>start=<%= LinkType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkType_view.sSrchWhere = "0=101" Then %>
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
<% If LinkType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
