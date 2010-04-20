<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zPage_view.aspx.vb" Inherits="zPage_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If zPage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var zPage_view = new ew_Page("zPage_view");
// page properties
zPage_view.PageID = "view"; // page ID
var EW_PAGE_ID = zPage_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
zPage_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Location
<br /><br />
<% If zPage.Export = "" Then %>
<a href="zPage_list.aspx">Back to List</a>&nbsp;
<a href="<%= zPage.AddUrl %>">Add</a>&nbsp;
<a href="<%= zPage.EditUrl %>">Edit</a>&nbsp;
<a href="<%= zPage.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= zPage.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% zPage_view.ShowMessage() %>
<p />
<% If zPage.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zPage_view.Pager Is Nothing Then zPage_view.Pager = New cPrevNextPager(zPage_view.lStartRec, zPage_view.lDisplayRecs, zPage_view.lTotalRecs) %>
<% If zPage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zPage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zPage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zPage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zPage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zPage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zPage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If zPage_view.sSrchWhere = "0=101" Then %>
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
<% If zPage.CompanyID.Visible Then ' CompanyID %>
	<tr<%= zPage.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= zPage.CompanyID.CellAttributes %>>
<div<%= zPage.CompanyID.ViewAttributes %>><%= zPage.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageOrder.Visible Then ' PageOrder %>
	<tr<%= zPage.PageOrder.RowAttributes %>>
		<td class="ewTableHeader">Order</td>
		<td<%= zPage.PageOrder.CellAttributes %>>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.GroupID.Visible Then ' GroupID %>
	<tr<%= zPage.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group</td>
		<td<%= zPage.GroupID.CellAttributes %>>
<div<%= zPage.GroupID.ViewAttributes %>><%= zPage.GroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
	<tr<%= zPage.ParentPageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Page</td>
		<td<%= zPage.ParentPageID.CellAttributes %>>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
	<tr<%= zPage.PageTypeID.RowAttributes %>>
		<td class="ewTableHeader">PageType</td>
		<td<%= zPage.PageTypeID.CellAttributes %>>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.Active.Visible Then ' Active %>
	<tr<%= zPage.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= zPage.Active.CellAttributes %>>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If zPage.zPageName.Visible Then ' PageName %>
	<tr<%= zPage.zPageName.RowAttributes %>>
		<td class="ewTableHeader">Page Name</td>
		<td<%= zPage.zPageName.CellAttributes %>>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageTitle.Visible Then ' PageTitle %>
	<tr<%= zPage.PageTitle.RowAttributes %>>
		<td class="ewTableHeader">Page Title</td>
		<td<%= zPage.PageTitle.CellAttributes %>>
<div<%= zPage.PageTitle.ViewAttributes %>><%= zPage.PageTitle.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageDescription.Visible Then ' PageDescription %>
	<tr<%= zPage.PageDescription.RowAttributes %>>
		<td class="ewTableHeader">Page Description</td>
		<td<%= zPage.PageDescription.CellAttributes %>>
<div<%= zPage.PageDescription.ViewAttributes %>><%= zPage.PageDescription.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageKeywords.Visible Then ' PageKeywords %>
	<tr<%= zPage.PageKeywords.RowAttributes %>>
		<td class="ewTableHeader">Page Keywords</td>
		<td<%= zPage.PageKeywords.CellAttributes %>>
<div<%= zPage.PageKeywords.ViewAttributes %>><%= zPage.PageKeywords.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.ImagesPerRow.Visible Then ' ImagesPerRow %>
	<tr<%= zPage.ImagesPerRow.RowAttributes %>>
		<td class="ewTableHeader">Images Per Row</td>
		<td<%= zPage.ImagesPerRow.CellAttributes %>>
<div<%= zPage.ImagesPerRow.ViewAttributes %>><%= zPage.ImagesPerRow.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.RowsPerPage.Visible Then ' RowsPerPage %>
	<tr<%= zPage.RowsPerPage.RowAttributes %>>
		<td class="ewTableHeader">Rows Per Page</td>
		<td<%= zPage.RowsPerPage.CellAttributes %>>
<div<%= zPage.RowsPerPage.ViewAttributes %>><%= zPage.RowsPerPage.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.PageFileName.Visible Then ' PageFileName %>
	<tr<%= zPage.PageFileName.RowAttributes %>>
		<td class="ewTableHeader">Page File Name</td>
		<td<%= zPage.PageFileName.CellAttributes %>>
<div<%= zPage.PageFileName.ViewAttributes %>><%= zPage.PageFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.VersionNo.Visible Then ' VersionNo %>
	<tr<%= zPage.VersionNo.RowAttributes %>>
		<td class="ewTableHeader">Version No</td>
		<td<%= zPage.VersionNo.CellAttributes %>>
<div<%= zPage.VersionNo.ViewAttributes %>><%= zPage.VersionNo.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= zPage.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>>
<div<%= zPage.SiteCategoryID.ViewAttributes %>><%= zPage.SiteCategoryID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= zPage.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group</td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>>
<div<%= zPage.SiteCategoryGroupID.ViewAttributes %>><%= zPage.SiteCategoryGroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zPage.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= zPage.ModifiedDT.RowAttributes %>>
		<td class="ewTableHeader">Modified DT</td>
		<td<%= zPage.ModifiedDT.CellAttributes %>>
<div<%= zPage.ModifiedDT.ViewAttributes %>><%= zPage.ModifiedDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If zPage.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zPage_view.Pager Is Nothing Then zPage_view.Pager = New cPrevNextPager(zPage_view.lStartRec, zPage_view.lDisplayRecs, zPage_view.lTotalRecs) %>
<% If zPage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zPage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zPage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zPage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zPage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zPage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zPage_view.PageUrl %>start=<%= zPage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zPage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If zPage_view.sSrchWhere = "0=101" Then %>
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
<% If zPage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
