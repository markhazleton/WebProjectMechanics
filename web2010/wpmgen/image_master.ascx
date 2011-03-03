<%@ Control ClassName="image_master" Language="VB" AutoEventWireup="false" CodeFile="image_master.ascx.vb" Inherits="image_master" %>
<p><span class="aspnetmaker"><%= ParentPage.Language.Phrase("MasterRecord") %><%= Image.TableCaption %><br />
<a href="<%= ParentPage.gsMasterReturnUrl %>"><%= ParentPage.Language.Phrase("BackToMasterPage") %></a></span></p>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= Image.ImageID.FldCaption %></td>
		<td valign="top"><%= Image.ImageName.FldCaption %></td>
		<td valign="top"><%= Image.ImageFileName.FldCaption %></td>
		<td valign="top"><%= Image.ImageThumbFileName.FldCaption %></td>
		<td valign="top"><%= Image.ImageDate.FldCaption %></td>
		<td valign="top"><%= Image.Active.FldCaption %></td>
		<td valign="top"><%= Image.ModifiedDT.FldCaption %></td>
		<td valign="top"><%= Image.VersionNo.FldCaption %></td>
		<td valign="top"><%= Image.ContactID.FldCaption %></td>
		<td valign="top"><%= Image.CompanyID.FldCaption %></td>
		<td valign="top"><%= Image.title.FldCaption %></td>
		<td valign="top"><%= Image.medium.FldCaption %></td>
		<td valign="top"><%= Image.size.FldCaption %></td>
		<td valign="top"><%= Image.price.FldCaption %></td>
		<td valign="top"><%= Image.color.FldCaption %></td>
		<td valign="top"><%= Image.subject.FldCaption %></td>
		<td valign="top"><%= Image.sold.FldCaption %></td>
	</tr>
</thead>
<tbody>
	<tr>
		<td<%= Image.ImageID.CellAttributes %>>
<div<%= Image.ImageID.ViewAttributes %>><%= Image.ImageID.ListViewValue %></div>
</td>
		<td<%= Image.ImageName.CellAttributes %>>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
</td>
		<td<%= Image.ImageFileName.CellAttributes %>>
<div<%= Image.ImageFileName.ViewAttributes %>><%= Image.ImageFileName.ListViewValue %></div>
</td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>>
<div<%= Image.ImageThumbFileName.ViewAttributes %>><%= Image.ImageThumbFileName.ListViewValue %></div>
</td>
		<td<%= Image.ImageDate.CellAttributes %>>
<div<%= Image.ImageDate.ViewAttributes %>><%= Image.ImageDate.ListViewValue %></div>
</td>
		<td<%= Image.Active.CellAttributes %>>
<% If Convert.ToString(Image.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Image.ModifiedDT.CellAttributes %>>
<div<%= Image.ModifiedDT.ViewAttributes %>><%= Image.ModifiedDT.ListViewValue %></div>
</td>
		<td<%= Image.VersionNo.CellAttributes %>>
<div<%= Image.VersionNo.ViewAttributes %>><%= Image.VersionNo.ListViewValue %></div>
</td>
		<td<%= Image.ContactID.CellAttributes %>>
<div<%= Image.ContactID.ViewAttributes %>><%= Image.ContactID.ListViewValue %></div>
</td>
		<td<%= Image.CompanyID.CellAttributes %>>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
</td>
		<td<%= Image.title.CellAttributes %>>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
</td>
		<td<%= Image.medium.CellAttributes %>>
<div<%= Image.medium.ViewAttributes %>><%= Image.medium.ListViewValue %></div>
</td>
		<td<%= Image.size.CellAttributes %>>
<div<%= Image.size.ViewAttributes %>><%= Image.size.ListViewValue %></div>
</td>
		<td<%= Image.price.CellAttributes %>>
<div<%= Image.price.ViewAttributes %>><%= Image.price.ListViewValue %></div>
</td>
		<td<%= Image.color.CellAttributes %>>
<div<%= Image.color.ViewAttributes %>><%= Image.color.ListViewValue %></div>
</td>
		<td<%= Image.subject.CellAttributes %>>
<div<%= Image.subject.ViewAttributes %>><%= Image.subject.ListViewValue %></div>
</td>
		<td<%= Image.sold.CellAttributes %>>
<% If Convert.ToString(Image.sold.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
</tbody>
</table>
</div>
</td></tr></table>
<br />
