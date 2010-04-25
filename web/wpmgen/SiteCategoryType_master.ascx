<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteCategoryType_master.ascx.vb" Inherits="SiteCategoryType_master" %>
<p><span class="aspnetmaker">Master Record: Site Type<br />
<a href="<%= ParentPage.gsMasterReturnUrl %>">Back to master page</a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
	<tr class="ewTableHeader">
		<td valign="top" style="white-space: nowrap;">Site Type</td>
		<td valign="top" style="white-space: nowrap;">File Name</td>
		<td valign="top" style="white-space: nowrap;">Transfer URL</td>
		<td valign="top" style="white-space: nowrap;">Default Category</td>
	</tr>
	<tr class="ewTableRow">
		<td style="white-space: nowrap;"<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeNM.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeNM.ListViewValue %></div>
</td>
		<td style="white-space: nowrap;"<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryFileName.ViewAttributes %>><%= SiteCategoryType.SiteCategoryFileName.ListViewValue %></div>
</td>
		<td style="white-space: nowrap;"<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTransferURL.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTransferURL.ListViewValue %></div>
</td>
		<td style="white-space: nowrap;"<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
<div<%= SiteCategoryType.DefaultSiteCategoryID.ViewAttributes %>><%= SiteCategoryType.DefaultSiteCategoryID.ListViewValue %></div>
</td>
	</tr>
</table>
</div>
</td></tr></table>
<br />
