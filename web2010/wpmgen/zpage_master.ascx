<%@ Control ClassName="zpage_master" Language="VB" AutoEventWireup="false" CodeFile="zpage_master.ascx.vb" Inherits="zpage_master" %>
<p><span class="aspnetmaker"><%= ParentPage.Language.Phrase("MasterRecord") %><%= zPage.TableCaption %><br />
<a href="<%= ParentPage.gsMasterReturnUrl %>"><%= ParentPage.Language.Phrase("BackToMasterPage") %></a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<thead>
	<tr class="ewTableHeader">
		<td valign="top" style="white-space: nowrap;"><%= zPage.ParentPageID.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.zPageName.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.PageTitle.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.PageTypeID.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.GroupID.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.Active.FldCaption %></td>
		<td valign="top" style="white-space: nowrap;"><%= zPage.PageOrder.FldCaption %></td>
	</tr>
</thead>
<tbody>
	<tr>
		<td<%= zPage.ParentPageID.CellAttributes %>>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ListViewValue %></div>
</td>
		<td<%= zPage.zPageName.CellAttributes %>>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ListViewValue %></div>
</td>
		<td<%= zPage.PageTitle.CellAttributes %>>
<div<%= zPage.PageTitle.ViewAttributes %>><%= zPage.PageTitle.ListViewValue %></div>
</td>
		<td<%= zPage.PageTypeID.CellAttributes %>>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ListViewValue %></div>
</td>
		<td<%= zPage.GroupID.CellAttributes %>>
<div<%= zPage.GroupID.ViewAttributes %>><%= zPage.GroupID.ListViewValue %></div>
</td>
		<td<%= zPage.Active.CellAttributes %>>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= zPage.PageOrder.CellAttributes %>>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ListViewValue %></div>
</td>
	</tr>
</tbody>
</table>
</div>
</td></tr></table>
<br />
