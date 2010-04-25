<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryType_view.aspx.vb" Inherits="SiteCategoryType_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategoryType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_view = new ew_Page("SiteCategoryType_view");
// page properties
SiteCategoryType_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteCategoryType_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Type
<br /><br />
<% If SiteCategoryType.Export = "" Then %>
<a href="SiteCategoryType_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteCategoryType.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteCategoryType.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteCategoryType.CopyUrl %>">Copy</a>&nbsp;
<% End If %>
</span></p>
<% SiteCategoryType_view.ShowMessage() %>
<p />
<% If SiteCategoryType.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryType_view.Pager Is Nothing Then SiteCategoryType_view.Pager = New cPrevNextPager(SiteCategoryType_view.lStartRec, SiteCategoryType_view.lDisplayRecs, SiteCategoryType_view.lTotalRecs) %>
<% If SiteCategoryType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategoryType_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategoryType.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteCategoryType.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Type ID</td>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeNM.Visible Then ' SiteCategoryTypeNM %>
	<tr<%= SiteCategoryType.SiteCategoryTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeNM.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeNM.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeDS.Visible Then ' SiteCategoryTypeDS %>
	<tr<%= SiteCategoryType.SiteCategoryTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeDS.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeDS.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryComment.Visible Then ' SiteCategoryComment %>
	<tr<%= SiteCategoryType.SiteCategoryComment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryComment.ViewAttributes %>><%= SiteCategoryType.SiteCategoryComment.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryFileName.Visible Then ' SiteCategoryFileName %>
	<tr<%= SiteCategoryType.SiteCategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">File Name</td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryFileName.ViewAttributes %>><%= SiteCategoryType.SiteCategoryFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTransferURL.Visible Then ' SiteCategoryTransferURL %>
	<tr<%= SiteCategoryType.SiteCategoryTransferURL.RowAttributes %>>
		<td class="ewTableHeader">Transfer URL</td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTransferURL.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTransferURL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategoryType.DefaultSiteCategoryID.Visible Then ' DefaultSiteCategoryID %>
	<tr<%= SiteCategoryType.DefaultSiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Default Category</td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
<div<%= SiteCategoryType.DefaultSiteCategoryID.ViewAttributes %>><%= SiteCategoryType.DefaultSiteCategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteCategoryType.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryType_view.Pager Is Nothing Then SiteCategoryType_view.Pager = New cPrevNextPager(SiteCategoryType_view.lStartRec, SiteCategoryType_view.lDisplayRecs, SiteCategoryType_view.lTotalRecs) %>
<% If SiteCategoryType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_view.PageUrl %>start=<%= SiteCategoryType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategoryType_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategoryType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
