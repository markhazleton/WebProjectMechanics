<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkRank_view.aspx.vb" Inherits="LinkRank_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkRank.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkRank_view = new ew_Page("LinkRank_view");
// page properties
LinkRank_view.PageID = "view"; // page ID
var EW_PAGE_ID = LinkRank_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Part Rank
<br /><br />
<% If LinkRank.Export = "" Then %>
<a href="LinkRank_list.aspx">Back to List</a>&nbsp;
<a href="<%= LinkRank.AddUrl %>">Add</a>&nbsp;
<a href="<%= LinkRank.EditUrl %>">Edit</a>&nbsp;
<a href="<%= LinkRank.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= LinkRank.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% LinkRank_view.ShowMessage() %>
<p />
<% If LinkRank.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkRank_view.Pager Is Nothing Then LinkRank_view.Pager = New cPrevNextPager(LinkRank_view.lStartRec, LinkRank_view.lDisplayRecs, LinkRank_view.lTotalRecs) %>
<% If LinkRank_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkRank_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkRank_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkRank_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkRank_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkRank_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkRank_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkRank_view.sSrchWhere = "0=101" Then %>
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
<% If LinkRank.ID.Visible Then ' ID %>
	<tr<%= LinkRank.ID.RowAttributes %>>
		<td class="ewTableHeader">ID</td>
		<td<%= LinkRank.ID.CellAttributes %>>
<div<%= LinkRank.ID.ViewAttributes %>><%= LinkRank.ID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkRank.LinkID.Visible Then ' LinkID %>
	<tr<%= LinkRank.LinkID.RowAttributes %>>
		<td class="ewTableHeader">Link ID</td>
		<td<%= LinkRank.LinkID.CellAttributes %>>
<div<%= LinkRank.LinkID.ViewAttributes %>><%= LinkRank.LinkID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkRank.UserID.Visible Then ' UserID %>
	<tr<%= LinkRank.UserID.RowAttributes %>>
		<td class="ewTableHeader">User ID</td>
		<td<%= LinkRank.UserID.CellAttributes %>>
<div<%= LinkRank.UserID.ViewAttributes %>><%= LinkRank.UserID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkRank.RankNum.Visible Then ' RankNum %>
	<tr<%= LinkRank.RankNum.RowAttributes %>>
		<td class="ewTableHeader">Rank Num</td>
		<td<%= LinkRank.RankNum.CellAttributes %>>
<div<%= LinkRank.RankNum.ViewAttributes %>><%= LinkRank.RankNum.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkRank.CateID.Visible Then ' CateID %>
	<tr<%= LinkRank.CateID.RowAttributes %>>
		<td class="ewTableHeader">Cate ID</td>
		<td<%= LinkRank.CateID.CellAttributes %>>
<div<%= LinkRank.CateID.ViewAttributes %>><%= LinkRank.CateID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If LinkRank.Comment.Visible Then ' Comment %>
	<tr<%= LinkRank.Comment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= LinkRank.Comment.CellAttributes %>>
<div<%= LinkRank.Comment.ViewAttributes %>><%= LinkRank.Comment.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If LinkRank.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkRank_view.Pager Is Nothing Then LinkRank_view.Pager = New cPrevNextPager(LinkRank_view.lStartRec, LinkRank_view.lDisplayRecs, LinkRank_view.lTotalRecs) %>
<% If LinkRank_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkRank_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkRank_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkRank_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkRank_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkRank_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkRank_view.PageUrl %>start=<%= LinkRank_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkRank_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If LinkRank_view.sSrchWhere = "0=101" Then %>
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
<% If LinkRank.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
