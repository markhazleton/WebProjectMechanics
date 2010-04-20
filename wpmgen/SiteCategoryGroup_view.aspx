<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryGroup_view.aspx.vb" Inherits="SiteCategoryGroup_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategoryGroup.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_view = new ew_Page("SiteCategoryGroup_view");
// page properties
SiteCategoryGroup_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteCategoryGroup_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Location Group
<br /><br />
<% If SiteCategoryGroup.Export = "" Then %>
<a href="SiteCategoryGroup_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteCategoryGroup.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteCategoryGroup.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteCategoryGroup.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= SiteCategoryGroup.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% SiteCategoryGroup_view.ShowMessage() %>
<p />
<% If SiteCategoryGroup.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryGroup_view.Pager Is Nothing Then SiteCategoryGroup_view.Pager = New cPrevNextPager(SiteCategoryGroup_view.lStartRec, SiteCategoryGroup_view.lDisplayRecs, SiteCategoryGroup_view.lTotalRecs) %>
<% If SiteCategoryGroup_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryGroup_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryGroup_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryGroup_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryGroup_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryGroup_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryGroup_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategoryGroup_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategoryGroup.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group ID</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupNM.Visible Then ' SiteCategoryGroupNM %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupNM.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group NM</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupNM.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupNM.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupDS.Visible Then ' SiteCategoryGroupDS %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupDS.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group DS</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupDS.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupDS.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupOrder.Visible Then ' SiteCategoryGroupOrder %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group Order</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteCategoryGroup.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryGroup_view.Pager Is Nothing Then SiteCategoryGroup_view.Pager = New cPrevNextPager(SiteCategoryGroup_view.lStartRec, SiteCategoryGroup_view.lDisplayRecs, SiteCategoryGroup_view.lTotalRecs) %>
<% If SiteCategoryGroup_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryGroup_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryGroup_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryGroup_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryGroup_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryGroup_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_view.PageUrl %>start=<%= SiteCategoryGroup_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryGroup_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategoryGroup_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategoryGroup.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
