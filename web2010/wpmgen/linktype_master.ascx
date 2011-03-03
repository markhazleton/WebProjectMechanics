<%@ Control ClassName="linktype_master" Language="VB" AutoEventWireup="false" CodeFile="linktype_master.ascx.vb" Inherits="linktype_master" %>
<p><span class="aspnetmaker"><%= ParentPage.Language.Phrase("MasterRecord") %><%= LinkType.TableCaption %><br />
<a href="<%= ParentPage.gsMasterReturnUrl %>"><%= ParentPage.Language.Phrase("BackToMasterPage") %></a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= LinkType.LinkTypeCD.FldCaption %></td>
		<td valign="top"><%= LinkType.LinkTypeDesc.FldCaption %></td>
		<td valign="top"><%= LinkType.LinkTypeTarget.FldCaption %></td>
	</tr>
</thead>
<tbody>
	<tr>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.ListViewValue %></div>
</td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
<div<%= LinkType.LinkTypeDesc.ViewAttributes %>><%= LinkType.LinkTypeDesc.ListViewValue %></div>
</td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
<div<%= LinkType.LinkTypeTarget.ViewAttributes %>><%= LinkType.LinkTypeTarget.ListViewValue %></div>
</td>
	</tr>
</tbody>
</table>
</div>
</td></tr></table>
<br />
