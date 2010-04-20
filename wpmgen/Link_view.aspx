<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Link_view.aspx.vb" Inherits="Link_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Link.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Link_view = new ew_Page("Link_view");
// page properties
Link_view.PageID = "view"; // page ID
var EW_PAGE_ID = Link_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Link_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Parts
<br /><br />
<% If Link.Export = "" Then %>
<a href="Link_list.aspx">Back to List</a>&nbsp;
<a href="<%= Link.AddUrl %>">Add</a>&nbsp;
<a href="<%= Link.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Link.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= Link.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% Link_view.ShowMessage() %>
<p />
<% If Link.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Link_view.Pager Is Nothing Then Link_view.Pager = New cPrevNextPager(Link_view.lStartRec, Link_view.lDisplayRecs, Link_view.lTotalRecs) %>
<% If Link_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Link_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Link_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Link_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Link_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Link_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Link_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Link_view.sSrchWhere = "0=101" Then %>
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
<% If Link.Title.Visible Then ' Title %>
	<tr<%= Link.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Link.Title.CellAttributes %>>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= Link.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Part Type</td>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.CategoryID.Visible Then ' CategoryID %>
	<tr<%= Link.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Part Category</td>
		<td<%= Link.CategoryID.CellAttributes %>>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Link.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Link.CompanyID.CellAttributes %>>
<div<%= Link.CompanyID.ViewAttributes %>><%= Link.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= Link.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Location Group</td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.zPageID.Visible Then ' PageID %>
	<tr<%= Link.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Link.zPageID.CellAttributes %>>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.Views.Visible Then ' Views %>
	<tr<%= Link.Views.RowAttributes %>>
		<td class="ewTableHeader">Visible/Active</td>
		<td<%= Link.Views.CellAttributes %>>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Link.Description.Visible Then ' Description %>
	<tr<%= Link.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Link.Description.CellAttributes %>>
<div<%= Link.Description.ViewAttributes %>><%= Link.Description.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.URL.Visible Then ' URL %>
	<tr<%= Link.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= Link.URL.CellAttributes %>>
<div<%= Link.URL.ViewAttributes %>><%= Link.URL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.Ranks.Visible Then ' Ranks %>
	<tr<%= Link.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= Link.Ranks.CellAttributes %>>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.UserID.Visible Then ' UserID %>
	<tr<%= Link.UserID.RowAttributes %>>
		<td class="ewTableHeader">User</td>
		<td<%= Link.UserID.CellAttributes %>>
<div<%= Link.UserID.ViewAttributes %>><%= Link.UserID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.ASIN.Visible Then ' ASIN %>
	<tr<%= Link.ASIN.RowAttributes %>>
		<td class="ewTableHeader">ASIN (Amazon)</td>
		<td<%= Link.ASIN.CellAttributes %>>
<div<%= Link.ASIN.ViewAttributes %>><%= Link.ASIN.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Link.DateAdd.Visible Then ' DateAdd %>
	<tr<%= Link.DateAdd.RowAttributes %>>
		<td class="ewTableHeader">Date Add</td>
		<td<%= Link.DateAdd.CellAttributes %>>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Link.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Link_view.Pager Is Nothing Then Link_view.Pager = New cPrevNextPager(Link_view.lStartRec, Link_view.lDisplayRecs, Link_view.lTotalRecs) %>
<% If Link_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Link_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Link_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Link_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Link_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Link_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Link_view.PageUrl %>start=<%= Link_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Link_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Link_view.sSrchWhere = "0=101" Then %>
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
<% If Link.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
