<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_view.aspx.vb" Inherits="CompanySiteTypeParameter_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If CompanySiteTypeParameter.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var CompanySiteTypeParameter_view = new ew_Page("CompanySiteTypeParameter_view");
// page properties
CompanySiteTypeParameter_view.PageID = "view"; // page ID
var EW_PAGE_ID = CompanySiteTypeParameter_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteTypeParameter_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteTypeParameter_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Type Parameter
<br /><br />
<% If CompanySiteTypeParameter.Export = "" Then %>
<a href="CompanySiteTypeParameter_list.aspx">Back to List</a>&nbsp;
<a href="<%= CompanySiteTypeParameter.AddUrl %>">Add</a>&nbsp;
<a href="<%= CompanySiteTypeParameter.EditUrl %>">Edit</a>&nbsp;
<a href="<%= CompanySiteTypeParameter.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= CompanySiteTypeParameter.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% CompanySiteTypeParameter_view.ShowMessage() %>
<p />
<% If CompanySiteTypeParameter.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteTypeParameter_view.Pager Is Nothing Then CompanySiteTypeParameter_view.Pager = New cPrevNextPager(CompanySiteTypeParameter_view.lStartRec, CompanySiteTypeParameter_view.lDisplayRecs, CompanySiteTypeParameter_view.lTotalRecs) %>
<% If CompanySiteTypeParameter_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteTypeParameter_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteTypeParameter_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteTypeParameter_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteTypeParameter_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteTypeParameter_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteTypeParameter_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If CompanySiteTypeParameter_view.sSrchWhere = "0=101" Then %>
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
<% If CompanySiteTypeParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<tr<%= CompanySiteTypeParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader">Parameter</td>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteParameterTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.CompanyID.Visible Then ' CompanyID %>
	<tr<%= CompanySiteTypeParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.CompanyID.ViewAttributes %>><%= CompanySiteTypeParameter.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SortOrder.Visible Then ' SortOrder %>
	<tr<%= CompanySiteTypeParameter.SortOrder.RowAttributes %>>
		<td class="ewTableHeader">Process Order</td>
		<td<%= CompanySiteTypeParameter.SortOrder.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SortOrder.ViewAttributes %>><%= CompanySiteTypeParameter.SortOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.ParameterValue.Visible Then ' ParameterValue %>
	<tr<%= CompanySiteTypeParameter.ParameterValue.RowAttributes %>>
		<td class="ewTableHeader">Parameter Value</td>
		<td<%= CompanySiteTypeParameter.ParameterValue.CellAttributes %>>
<div<%= CompanySiteTypeParameter.ParameterValue.ViewAttributes %>><%= CompanySiteTypeParameter.ParameterValue.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If CompanySiteTypeParameter.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteTypeParameter_view.Pager Is Nothing Then CompanySiteTypeParameter_view.Pager = New cPrevNextPager(CompanySiteTypeParameter_view.lStartRec, CompanySiteTypeParameter_view.lDisplayRecs, CompanySiteTypeParameter_view.lTotalRecs) %>
<% If CompanySiteTypeParameter_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteTypeParameter_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteTypeParameter_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteTypeParameter_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteTypeParameter_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteTypeParameter_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_view.PageUrl %>start=<%= CompanySiteTypeParameter_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteTypeParameter_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If CompanySiteTypeParameter_view.sSrchWhere = "0=101" Then %>
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
<% If CompanySiteTypeParameter.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
