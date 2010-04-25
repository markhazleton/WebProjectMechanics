<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Company_view.aspx.vb" Inherits="Company_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Company.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Company_view = new ew_Page("Company_view");
// page properties
Company_view.PageID = "view"; // page ID
var EW_PAGE_ID = Company_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Company_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site
<br /><br />
<% If Company.Export = "" Then %>
<a href="Company_list.aspx">Back to List</a>&nbsp;
<a href="<%= Company.AddUrl %>">Add</a>&nbsp;
<a href="<%= Company.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Company.CopyUrl %>">Copy</a>&nbsp;
<% End If %>
</span></p>
<% Company_view.ShowMessage() %>
<p />
<% If Company.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Company_view.Pager Is Nothing Then Company_view.Pager = New cPrevNextPager(Company_view.lStartRec, Company_view.lDisplayRecs, Company_view.lTotalRecs) %>
<% If Company_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Company_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Company_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Company_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Company_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Company_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Company_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Company_view.sSrchWhere = "0=101" Then %>
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
<% If Company.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Company.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">SetupID</td>
		<td<%= Company.CompanyID.CellAttributes %>>
<div<%= Company.CompanyID.ViewAttributes %>><%= Company.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.CompanyName.Visible Then ' CompanyName %>
	<tr<%= Company.CompanyName.RowAttributes %>>
		<td class="ewTableHeader">Company Name</td>
		<td<%= Company.CompanyName.CellAttributes %>>
<div<%= Company.CompanyName.ViewAttributes %>><%= Company.CompanyName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.SiteTitle.Visible Then ' SiteTitle %>
	<tr<%= Company.SiteTitle.RowAttributes %>>
		<td class="ewTableHeader">Site Title</td>
		<td<%= Company.SiteTitle.CellAttributes %>>
<div<%= Company.SiteTitle.ViewAttributes %>><%= Company.SiteTitle.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.SiteURL.Visible Then ' SiteURL %>
	<tr<%= Company.SiteURL.RowAttributes %>>
		<td class="ewTableHeader">Site URL</td>
		<td<%= Company.SiteURL.CellAttributes %>>
<div<%= Company.SiteURL.ViewAttributes %>><%= Company.SiteURL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.GalleryFolder.Visible Then ' GalleryFolder %>
	<tr<%= Company.GalleryFolder.RowAttributes %>>
		<td class="ewTableHeader">Gallery Folder</td>
		<td<%= Company.GalleryFolder.CellAttributes %>>
<div<%= Company.GalleryFolder.ViewAttributes %>><%= Company.GalleryFolder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= Company.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= Company.SiteCategoryTypeID.CellAttributes %>>
<div<%= Company.SiteCategoryTypeID.ViewAttributes %>><%= Company.SiteCategoryTypeID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.HomePageID.Visible Then ' HomePageID %>
	<tr<%= Company.HomePageID.RowAttributes %>>
		<td class="ewTableHeader">Home Page</td>
		<td<%= Company.HomePageID.CellAttributes %>>
<div<%= Company.HomePageID.ViewAttributes %>><%= Company.HomePageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.DefaultArticleID.Visible Then ' DefaultArticleID %>
	<tr<%= Company.DefaultArticleID.RowAttributes %>>
		<td class="ewTableHeader">Default Article</td>
		<td<%= Company.DefaultArticleID.CellAttributes %>>
<div<%= Company.DefaultArticleID.ViewAttributes %>><%= Company.DefaultArticleID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.SiteTemplate.Visible Then ' SiteTemplate %>
	<tr<%= Company.SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader">Template</td>
		<td<%= Company.SiteTemplate.CellAttributes %>>
<div<%= Company.SiteTemplate.ViewAttributes %>><%= Company.SiteTemplate.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.DefaultSiteTemplate.Visible Then ' DefaultSiteTemplate %>
	<tr<%= Company.DefaultSiteTemplate.RowAttributes %>>
		<td class="ewTableHeader">Default Template</td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
<div<%= Company.DefaultSiteTemplate.ViewAttributes %>><%= Company.DefaultSiteTemplate.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.UseBreadCrumbURL.Visible Then ' UseBreadCrumbURL %>
	<tr<%= Company.UseBreadCrumbURL.RowAttributes %>>
		<td class="ewTableHeader">Use Bread Crumb URL</td>
		<td<%= Company.UseBreadCrumbURL.CellAttributes %>>
<% If Convert.ToString(Company.UseBreadCrumbURL.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Company.UseBreadCrumbURL.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Company.UseBreadCrumbURL.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Company.SingleSiteGallery.Visible Then ' SingleSiteGallery %>
	<tr<%= Company.SingleSiteGallery.RowAttributes %>>
		<td class="ewTableHeader">Single Site Gallery</td>
		<td<%= Company.SingleSiteGallery.CellAttributes %>>
<% If Convert.ToString(Company.SingleSiteGallery.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Company.SingleSiteGallery.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Company.SingleSiteGallery.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Company.ActiveFL.Visible Then ' ActiveFL %>
	<tr<%= Company.ActiveFL.RowAttributes %>>
		<td class="ewTableHeader">Active FL</td>
		<td<%= Company.ActiveFL.CellAttributes %>>
<% If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Company.ActiveFL.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Company.ActiveFL.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Company.Address.Visible Then ' Address %>
	<tr<%= Company.Address.RowAttributes %>>
		<td class="ewTableHeader">Address</td>
		<td<%= Company.Address.CellAttributes %>>
<div<%= Company.Address.ViewAttributes %>><%= Company.Address.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.City.Visible Then ' City %>
	<tr<%= Company.City.RowAttributes %>>
		<td class="ewTableHeader">City</td>
		<td<%= Company.City.CellAttributes %>>
<div<%= Company.City.ViewAttributes %>><%= Company.City.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.StateOrProvince.Visible Then ' StateOrProvince %>
	<tr<%= Company.StateOrProvince.RowAttributes %>>
		<td class="ewTableHeader">State/Province</td>
		<td<%= Company.StateOrProvince.CellAttributes %>>
<div<%= Company.StateOrProvince.ViewAttributes %>><%= Company.StateOrProvince.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.PostalCode.Visible Then ' PostalCode %>
	<tr<%= Company.PostalCode.RowAttributes %>>
		<td class="ewTableHeader">Postal Code</td>
		<td<%= Company.PostalCode.CellAttributes %>>
<div<%= Company.PostalCode.ViewAttributes %>><%= Company.PostalCode.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.Country.Visible Then ' Country %>
	<tr<%= Company.Country.RowAttributes %>>
		<td class="ewTableHeader">Country</td>
		<td<%= Company.Country.CellAttributes %>>
<div<%= Company.Country.ViewAttributes %>><%= Company.Country.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.Component.Visible Then ' Component %>
	<tr<%= Company.Component.RowAttributes %>>
		<td class="ewTableHeader">Component</td>
		<td<%= Company.Component.CellAttributes %>>
<div<%= Company.Component.ViewAttributes %>><%= Company.Component.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.FromEmail.Visible Then ' FromEmail %>
	<tr<%= Company.FromEmail.RowAttributes %>>
		<td class="ewTableHeader">From Email</td>
		<td<%= Company.FromEmail.CellAttributes %>>
<div<%= Company.FromEmail.ViewAttributes %>><%= Company.FromEmail.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Company.SMTP.Visible Then ' SMTP %>
	<tr<%= Company.SMTP.RowAttributes %>>
		<td class="ewTableHeader">SMTP</td>
		<td<%= Company.SMTP.CellAttributes %>>
<div<%= Company.SMTP.ViewAttributes %>><%= Company.SMTP.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Company.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Company_view.Pager Is Nothing Then Company_view.Pager = New cPrevNextPager(Company_view.lStartRec, Company_view.lDisplayRecs, Company_view.lTotalRecs) %>
<% If Company_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Company_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Company_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Company_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Company_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Company_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Company_view.PageUrl %>start=<%= Company_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Company_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Company_view.sSrchWhere = "0=101" Then %>
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
<% If Company.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
