<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageAlias_view.aspx.vb" Inherits="PageAlias_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageAlias.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageAlias_view = new ew_Page("PageAlias_view");
// page properties
PageAlias_view.PageID = "view"; // page ID
var EW_PAGE_ID = PageAlias_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageAlias_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Location Alias
<br /><br />
<% If PageAlias.Export = "" Then %>
<a href="PageAlias_list.aspx">Back to List</a>&nbsp;
<a href="<%= PageAlias.AddUrl %>">Add</a>&nbsp;
<a href="<%= PageAlias.EditUrl %>">Edit</a>&nbsp;
<a href="<%= PageAlias.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= PageAlias.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% PageAlias_view.ShowMessage() %>
<p />
<% If PageAlias.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageAlias_view.Pager Is Nothing Then PageAlias_view.Pager = New cPrevNextPager(PageAlias_view.lStartRec, PageAlias_view.lDisplayRecs, PageAlias_view.lTotalRecs) %>
<% If PageAlias_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageAlias_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageAlias_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageAlias_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageAlias_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageAlias_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageAlias_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageAlias_view.sSrchWhere = "0=101" Then %>
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
<% If PageAlias.zPageURL.Visible Then ' PageURL %>
	<tr<%= PageAlias.zPageURL.RowAttributes %>>
		<td class="ewTableHeader">Page URL</td>
		<td<%= PageAlias.zPageURL.CellAttributes %>>
<div<%= PageAlias.zPageURL.ViewAttributes %>><%= PageAlias.zPageURL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageAlias.TargetURL.Visible Then ' TargetURL %>
	<tr<%= PageAlias.TargetURL.RowAttributes %>>
		<td class="ewTableHeader">Target URL</td>
		<td<%= PageAlias.TargetURL.CellAttributes %>>
<div<%= PageAlias.TargetURL.ViewAttributes %>><%= PageAlias.TargetURL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageAlias.AliasType.Visible Then ' AliasType %>
	<tr<%= PageAlias.AliasType.RowAttributes %>>
		<td class="ewTableHeader">Alias Type</td>
		<td<%= PageAlias.AliasType.CellAttributes %>>
<div<%= PageAlias.AliasType.ViewAttributes %>><%= PageAlias.AliasType.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageAlias.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageAlias.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= PageAlias.CompanyID.CellAttributes %>>
<div<%= PageAlias.CompanyID.ViewAttributes %>><%= PageAlias.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If PageAlias.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageAlias_view.Pager Is Nothing Then PageAlias_view.Pager = New cPrevNextPager(PageAlias_view.lStartRec, PageAlias_view.lDisplayRecs, PageAlias_view.lTotalRecs) %>
<% If PageAlias_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageAlias_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageAlias_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageAlias_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageAlias_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageAlias_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageAlias_view.PageUrl %>start=<%= PageAlias_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageAlias_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageAlias_view.sSrchWhere = "0=101" Then %>
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
<% If PageAlias.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
