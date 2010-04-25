<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteParameterType_master.ascx.vb" Inherits="SiteParameterType_master" %>
<p><span class="aspnetmaker">Master Record: Parameter Type<br />
<a href="<%= ParentPage.gsMasterReturnUrl %>">Back to master page</a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
	<tr class="ewTableHeader">
		<td valign="top">Parameter Name</td>
		<td valign="top">Parameter Description</td>
		<td valign="top">Parameter Order</td>
	</tr>
	<tr class="ewTableRow">
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeNM.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeNM.ListViewValue %></div>
</td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeDS.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeDS.ListViewValue %></div>
</td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeOrder.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeOrder.ListViewValue %></div>
</td>
	</tr>
</table>
</div>
</td></tr></table>
<br />
