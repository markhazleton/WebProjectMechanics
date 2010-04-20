<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageType_view.aspx.vb" Inherits="PageType_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageType_view = new ew_Page("PageType_view");
// page properties
PageType_view.PageID = "view"; // page ID
var EW_PAGE_ID = PageType_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageType_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Location Type
<br /><br />
<% If PageType.Export = "" Then %>
<a href="PageType_list.aspx">Back to List</a>&nbsp;
<a href="<%= PageType.AddUrl %>">Add</a>&nbsp;
<a href="<%= PageType.EditUrl %>">Edit</a>&nbsp;
<a href="<%= PageType.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= PageType.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% PageType_view.ShowMessage() %>
<p />
<% If PageType.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageType_view.Pager Is Nothing Then PageType_view.Pager = New cPrevNextPager(PageType_view.lStartRec, PageType_view.lDisplayRecs, PageType_view.lTotalRecs) %>
<% If PageType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageType_view.sSrchWhere = "0=101" Then %>
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
<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
	<tr<%= PageType.PageTypeID.RowAttributes %>>
		<td class="ewTableHeader">Page Type ID</td>
		<td<%= PageType.PageTypeID.CellAttributes %>>
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
	<tr<%= PageType.PageTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Page Type CD</td>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
<div<%= PageType.PageTypeCD.ViewAttributes %>><%= PageType.PageTypeCD.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
	<tr<%= PageType.PageTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Page Type Desc</td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
<div<%= PageType.PageTypeDesc.ViewAttributes %>><%= PageType.PageTypeDesc.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageType.PageFileName.Visible Then ' PageFileName %>
	<tr<%= PageType.PageFileName.RowAttributes %>>
		<td class="ewTableHeader">Page File Name</td>
		<td<%= PageType.PageFileName.CellAttributes %>>
<div<%= PageType.PageFileName.ViewAttributes %>><%= PageType.PageFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If PageType.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageType_view.Pager Is Nothing Then PageType_view.Pager = New cPrevNextPager(PageType_view.lStartRec, PageType_view.lDisplayRecs, PageType_view.lTotalRecs) %>
<% If PageType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageType_view.PageUrl %>start=<%= PageType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageType_view.sSrchWhere = "0=101" Then %>
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
<% If PageType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
