<%@ Control ClassName="company_master" Language="VB" AutoEventWireup="false" CodeFile="company_master.ascx.vb" Inherits="company_master" %>
<p><span class="aspnetmaker"><%= ParentPage.Language.Phrase("MasterRecord") %><%= Company.TableCaption %><br />
<a href="<%= ParentPage.gsMasterReturnUrl %>"><%= ParentPage.Language.Phrase("BackToMasterPage") %></a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= Company.CompanyName.FldCaption %></td>
		<td valign="top"><%= Company.SiteTitle.FldCaption %></td>
		<td valign="top"><%= Company.GalleryFolder.FldCaption %></td>
		<td valign="top"><%= Company.SiteTemplate.FldCaption %></td>
		<td valign="top"><%= Company.DefaultSiteTemplate.FldCaption %></td>
	</tr>
</thead>
<tbody>
	<tr>
		<td<%= Company.CompanyName.CellAttributes %>>
<div<%= Company.CompanyName.ViewAttributes %>><%= Company.CompanyName.ListViewValue %></div>
</td>
		<td<%= Company.SiteTitle.CellAttributes %>>
<div<%= Company.SiteTitle.ViewAttributes %>><%= Company.SiteTitle.ListViewValue %></div>
</td>
		<td<%= Company.GalleryFolder.CellAttributes %>>
<div<%= Company.GalleryFolder.ViewAttributes %>><%= Company.GalleryFolder.ListViewValue %></div>
</td>
		<td<%= Company.SiteTemplate.CellAttributes %>>
<div<%= Company.SiteTemplate.ViewAttributes %>><%= Company.SiteTemplate.ListViewValue %></div>
</td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
<div<%= Company.DefaultSiteTemplate.ViewAttributes %>><%= Company.DefaultSiteTemplate.ListViewValue %></div>
</td>
	</tr>
</tbody>
</table>
</div>
</td></tr></table>
<br />
