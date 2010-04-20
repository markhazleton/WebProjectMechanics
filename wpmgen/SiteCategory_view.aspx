<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategory_view.aspx.vb" Inherits="SiteCategory_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategory.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_view = new ew_Page("SiteCategory_view");
// page properties
SiteCategory_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteCategory_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Type Location
<br /><br />
<% If SiteCategory.Export = "" Then %>
<a href="SiteCategory_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteCategory.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteCategory.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteCategory.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= SiteCategory.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% SiteCategory_view.ShowMessage() %>
<p />
<% If SiteCategory.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategory_view.Pager Is Nothing Then SiteCategory_view.Pager = New cPrevNextPager(SiteCategory_view.lStartRec, SiteCategory_view.lDisplayRecs, SiteCategory_view.lTotalRecs) %>
<% If SiteCategory_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategory_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategory_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategory_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategory_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategory_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategory_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategory_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteCategory.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
	<tr<%= SiteCategory.GroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Order</td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
	<tr<%= SiteCategory.CategoryName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryTitle.Visible Then ' CategoryTitle %>
	<tr<%= SiteCategory.CategoryTitle.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>>
<div<%= SiteCategory.CategoryTitle.ViewAttributes %>><%= SiteCategory.CategoryTitle.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryDescription.Visible Then ' CategoryDescription %>
	<tr<%= SiteCategory.CategoryDescription.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>>
<div<%= SiteCategory.CategoryDescription.ViewAttributes %>><%= SiteCategory.CategoryDescription.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryKeywords.Visible Then ' CategoryKeywords %>
	<tr<%= SiteCategory.CategoryKeywords.RowAttributes %>>
		<td class="ewTableHeader">Keywords</td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>>
<div<%= SiteCategory.CategoryKeywords.ViewAttributes %>><%= SiteCategory.CategoryKeywords.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
	<tr<%= SiteCategory.ParentCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Parent Category</td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
	<tr<%= SiteCategory.CategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">Category File Name</td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteCategory.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteCategory.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategory_view.Pager Is Nothing Then SiteCategory_view.Pager = New cPrevNextPager(SiteCategory_view.lStartRec, SiteCategory_view.lDisplayRecs, SiteCategory_view.lTotalRecs) %>
<% If SiteCategory_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategory_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategory_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategory_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategory_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategory_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategory_view.PageUrl %>start=<%= SiteCategory_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategory_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteCategory_view.sSrchWhere = "0=101" Then %>
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
<% If SiteCategory.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
