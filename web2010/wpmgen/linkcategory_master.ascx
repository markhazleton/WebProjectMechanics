<%@ Control ClassName="linkcategory_master" Language="VB" AutoEventWireup="false" CodeFile="linkcategory_master.ascx.vb" Inherits="linkcategory_master" %>
<p><span class="aspnetmaker"><%= ParentPage.Language.Phrase("MasterRecord") %><%= LinkCategory.TableCaption %><br />
<a href="<%= ParentPage.gsMasterReturnUrl %>"><%= ParentPage.Language.Phrase("BackToMasterPage") %></a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= LinkCategory.ID.FldCaption %></td>
		<td valign="top"><%= LinkCategory.Title.FldCaption %></td>
		<td valign="top"><%= LinkCategory.ParentID.FldCaption %></td>
		<td valign="top"><%= LinkCategory.zPageID.FldCaption %></td>
	</tr>
</thead>
<tbody>
	<tr>
		<td<%= LinkCategory.ID.CellAttributes %>>
<div<%= LinkCategory.ID.ViewAttributes %>><%= LinkCategory.ID.ListViewValue %></div>
</td>
		<td<%= LinkCategory.Title.CellAttributes %>>
<div<%= LinkCategory.Title.ViewAttributes %>><%= LinkCategory.Title.ListViewValue %></div>
</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
<div<%= LinkCategory.ParentID.ViewAttributes %>><%= LinkCategory.ParentID.ListViewValue %></div>
</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
<div<%= LinkCategory.zPageID.ViewAttributes %>><%= LinkCategory.zPageID.ListViewValue %></div>
</td>
	</tr>
</tbody>
</table>
</div>
</td></tr></table>
<br />
