<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteLink_view.aspx.vb" Inherits="SiteLink_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteLink.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteLink_view = new ew_Page("SiteLink_view");
// page properties
SiteLink_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteLink_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Type Parts
<br /><br />
<% If SiteLink.Export = "" Then %>
<a href="SiteLink_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteLink.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteLink.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteLink.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= SiteLink.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% SiteLink_view.ShowMessage() %>
<p />
<% If SiteLink.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteLink_view.Pager Is Nothing Then SiteLink_view.Pager = New cPrevNextPager(SiteLink_view.lStartRec, SiteLink_view.lDisplayRecs, SiteLink_view.lTotalRecs) %>
<% If SiteLink_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteLink_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteLink_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteLink_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteLink_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteLink_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteLink_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteLink_view.sSrchWhere = "0=101" Then %>
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
<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteLink.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryTypeID.ViewAttributes %>><%= SiteLink.SiteCategoryTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.Title.Visible Then ' Title %>
	<tr<%= SiteLink.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= SiteLink.Title.CellAttributes %>>
<div<%= SiteLink.Title.ViewAttributes %>><%= SiteLink.Title.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= SiteLink.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type</td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
<div<%= SiteLink.LinkTypeCD.ViewAttributes %>><%= SiteLink.LinkTypeCD.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
	<tr<%= SiteLink.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Link Category</td>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
<div<%= SiteLink.CategoryID.ViewAttributes %>><%= SiteLink.CategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
	<tr<%= SiteLink.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
<div<%= SiteLink.CompanyID.ViewAttributes %>><%= SiteLink.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= SiteLink.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryID.ViewAttributes %>><%= SiteLink.SiteCategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteLink.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryGroupID.ViewAttributes %>><%= SiteLink.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.Description.Visible Then ' Description %>
	<tr<%= SiteLink.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteLink.Description.CellAttributes %>>
<div<%= SiteLink.Description.ViewAttributes %>><%= SiteLink.Description.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.URL.Visible Then ' URL %>
	<tr<%= SiteLink.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= SiteLink.URL.CellAttributes %>>
<div<%= SiteLink.URL.ViewAttributes %>><%= SiteLink.URL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.Ranks.Visible Then ' Ranks %>
	<tr<%= SiteLink.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= SiteLink.Ranks.CellAttributes %>>
<div<%= SiteLink.Ranks.ViewAttributes %>><%= SiteLink.Ranks.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteLink.Views.Visible Then ' Views %>
	<tr<%= SiteLink.Views.RowAttributes %>>
		<td class="ewTableHeader">Active/Visible</td>
		<td<%= SiteLink.Views.CellAttributes %>>
<% If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= SiteLink.Views.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= SiteLink.Views.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteLink.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteLink_view.Pager Is Nothing Then SiteLink_view.Pager = New cPrevNextPager(SiteLink_view.lStartRec, SiteLink_view.lDisplayRecs, SiteLink_view.lTotalRecs) %>
<% If SiteLink_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteLink_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteLink_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteLink_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteLink_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteLink_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteLink_view.PageUrl %>start=<%= SiteLink_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteLink_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteLink_view.sSrchWhere = "0=101" Then %>
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
<% If SiteLink.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
