<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteParameter_view.aspx.vb" Inherits="CompanySiteParameter_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If CompanySiteParameter.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var CompanySiteParameter_view = new ew_Page("CompanySiteParameter_view");
// page properties
CompanySiteParameter_view.PageID = "view"; // page ID
var EW_PAGE_ID = CompanySiteParameter_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteParameter_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteParameter_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Parameter
<br /><br />
<% If CompanySiteParameter.Export = "" Then %>
<a href="CompanySiteParameter_list.aspx">Back to List</a>&nbsp;
<a href="<%= CompanySiteParameter.AddUrl %>">Add</a>&nbsp;
<a href="<%= CompanySiteParameter.EditUrl %>">Edit</a>&nbsp;
<a href="<%= CompanySiteParameter.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= CompanySiteParameter.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% CompanySiteParameter_view.ShowMessage() %>
<p />
<% If CompanySiteParameter.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteParameter_view.Pager Is Nothing Then CompanySiteParameter_view.Pager = New cPrevNextPager(CompanySiteParameter_view.lStartRec, CompanySiteParameter_view.lDisplayRecs, CompanySiteParameter_view.lTotalRecs) %>
<% If CompanySiteParameter_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteParameter_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteParameter_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteParameter_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteParameter_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteParameter_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteParameter_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If CompanySiteParameter_view.sSrchWhere = "0=101" Then %>
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
<% If CompanySiteParameter.CompanyID.Visible Then ' CompanyID %>
	<tr<%= CompanySiteParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>>
<div<%= CompanySiteParameter.CompanyID.ViewAttributes %>><%= CompanySiteParameter.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteParameter.zPageID.Visible Then ' PageID %>
	<tr<%= CompanySiteParameter.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>>
<div<%= CompanySiteParameter.zPageID.ViewAttributes %>><%= CompanySiteParameter.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= CompanySiteParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>>
<div<%= CompanySiteParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteParameter.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<tr<%= CompanySiteParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader"> Parameter</td>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SortOrder.Visible Then ' SortOrder %>
	<tr<%= CompanySiteParameter.SortOrder.RowAttributes %>>
		<td class="ewTableHeader">Process Order</td>
		<td<%= CompanySiteParameter.SortOrder.CellAttributes %>>
<div<%= CompanySiteParameter.SortOrder.ViewAttributes %>><%= CompanySiteParameter.SortOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteParameter.ParameterValue.Visible Then ' ParameterValue %>
	<tr<%= CompanySiteParameter.ParameterValue.RowAttributes %>>
		<td class="ewTableHeader">Parameter Value</td>
		<td<%= CompanySiteParameter.ParameterValue.CellAttributes %>>
<div<%= CompanySiteParameter.ParameterValue.ViewAttributes %>><%= CompanySiteParameter.ParameterValue.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If CompanySiteParameter.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteParameter_view.Pager Is Nothing Then CompanySiteParameter_view.Pager = New cPrevNextPager(CompanySiteParameter_view.lStartRec, CompanySiteParameter_view.lDisplayRecs, CompanySiteParameter_view.lTotalRecs) %>
<% If CompanySiteParameter_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteParameter_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteParameter_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteParameter_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteParameter_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteParameter_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_view.PageUrl %>start=<%= CompanySiteParameter_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteParameter_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If CompanySiteParameter_view.sSrchWhere = "0=101" Then %>
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
<% If CompanySiteParameter.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
